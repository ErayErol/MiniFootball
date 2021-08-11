﻿namespace MiniFootball.Services.Games
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using MiniFootball.Models;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GameService : IGameService
    {
        private readonly MiniFootballDbContext data;
        private readonly IConfigurationProvider mapper;

        public GameService(
            MiniFootballDbContext data,
            IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public GameQueryServiceModel All(
            string cityName,
            string searchTerm,
            Sorting sorting,
            int currentPage,
            int gamesPerPage)
        {
            var city = this.data
                .Cities
                .FirstOrDefault(c => c.Name == cityName);

            var gamesQuery = this.data.Games.AsQueryable();

            if (string.IsNullOrWhiteSpace(city?.Name) == false)
            {
                gamesQuery = gamesQuery
                    .Where(g => g.Field.City.Name == city.Name);
            }

            if (string.IsNullOrWhiteSpace(searchTerm) == false)
            {
                gamesQuery = gamesQuery
                    .Where(g => g.Field
                        .Name
                        .ToLower()
                        .Contains(searchTerm.ToLower()));
            }

            // TODO: You can add searching by time too
            gamesQuery = sorting switch
            {
                Sorting.City
                    => gamesQuery
                        .OrderBy(g => g.Field.City),
                Sorting.FieldName
                    => gamesQuery
                        .OrderBy(g => g.Field.Name),
                Sorting.DateCreated or _
                    => gamesQuery
                        .OrderByDescending(g => g.Date.Date)
            };

            var totalGames = gamesQuery.Count();

            var games = GetGames(gamesQuery
                .Skip((currentPage - 1) * gamesPerPage)
                .Take(gamesPerPage), this.mapper)
                .ToList();

            foreach (var gameListingServiceModel in games)
            {
                gameListingServiceModel.Field.Country = data.Countries.Find(gameListingServiceModel.Field.CountryId);
                gameListingServiceModel.Field.City = data.Cities.Find(gameListingServiceModel.Field.CityId);
            }

            return new GameQueryServiceModel
            {
                CurrentPage = currentPage,
                TotalGames = totalGames,
                GamesPerPage = gamesPerPage,
                Games = games,
            };
        }

        public string Create(int fieldId,
            DateTime date,
            int time,
            int numberOfPlayers,
            string facebookUrl,
            bool ball,
            bool jerseys,
            bool goalkeeper,
            string description,
            int places,
            bool hasPlaces,
            int adminId)
        {
            var game = new Game
            {
                FieldId = fieldId,
                Date = date,
                Time = time,
                NumberOfPlayers = numberOfPlayers,
                FacebookUrl = facebookUrl,
                Ball = ball,
                Jerseys = jerseys,
                Goalkeeper = goalkeeper,
                Description = description,
                Places = places,
                HasPlaces = hasPlaces,
                AdminId = adminId,
            };

            this.data.Games.Add(game);
            this.data.SaveChanges();

            return game.Id;
        }

        public bool Edit(string id,
            DateTime? date,
            int? time,
            int? numberOfPlayers,
            string facebookUrl,
            bool ball,
            bool jerseys,
            bool goalkeeper,
            string description)
        {
            var game = this.data.Games.Find(id);

            if (game == null)
            {
                return false;
            }

            if (game.NumberOfPlayers != numberOfPlayers.Value)
            {
                game.Places = numberOfPlayers.Value;

                var joinedPlayers = SeePlayers(game.Id).Count();
                game.Places -= joinedPlayers;
            }

            game.Date = date.Value;
            game.Time = time.Value;
            game.NumberOfPlayers = numberOfPlayers.Value;
            game.FacebookUrl = facebookUrl;
            game.Ball = ball;
            game.Jerseys = jerseys;
            game.Goalkeeper = goalkeeper;
            game.Description = description;

            this.data.SaveChanges();

            return true;
        }

        public bool AddUserToGame(string id, string userId)
        {
            var game = this.data
                .Games
                .FirstOrDefault(c => c.Id == id);

            if (game == null)
            {
                return false;
            }

            if (game.HasPlaces)
            {
                game.Places--;
            }

            var userGame = new UserGame
            {
                GameId = game.Id,
                UserId = userId
            };

            this.data.UserGames.Add(userGame);

            this.data.SaveChanges();

            return true;
        }

        public bool IsUserIsJoinGame(string id, string userId)
            => this.data.UserGames
                .Any(c => c.GameId == id && c.UserId == userId);

        public IQueryable<GameSeePlayersServiceModel> SeePlayers(string id)
            => this.data
                .UserGames
                .Where(g => g.GameId == id)
                .Select(g => new GameSeePlayersServiceModel
                {
                    UserId = g.User.Id,
                    ImageUrl = g.User.ImageUrl,
                    FirstName = g.User.FirstName,
                    LastName = g.User.LastName,
                    NickName = g.User.NickName,
                    PhoneNumber = g.User.PhoneNumber,
                    IsCreator = this.GameIdUserId(id).UserId == g.UserId,
                });


        public bool Delete(string id)
        {
            var game = this.data
                .Games
                .FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return false;
            }

            this.data.Remove(game);
            this.data.SaveChanges();

            return true;
        }

        public GameIdUserIdServiceModel GameIdUserId(string id)
            => this.data
                .Games
                .Where(g => g.Id == id)
                .ProjectTo<GameIdUserIdServiceModel>(this.mapper)
                .FirstOrDefault();

        public bool IsExist(int fieldId, DateTime date, int time)
        {
            var any = this.data.Games
                .Any(g => g.FieldId.Equals(fieldId) && g.Date.Equals(date) && g.Time.Equals(time));

            return any;
        }

        public IEnumerable<GameListingServiceModel> ByUser(string userId)
        {
            var games = GetGames(
                this.data
                    .Games
                    .Where(g => g.Admin.UserId == userId),
                this.mapper);

            foreach (var gameListingServiceModel in games)
            {
                gameListingServiceModel.Field.Country = data.Countries.Find(gameListingServiceModel.Field.CountryId);
                gameListingServiceModel.Field.City = data.Cities.Find(gameListingServiceModel.Field.CityId);
            }

            return games;
        }

        public IEnumerable<GameListingServiceModel> Latest()
            => this.data
                .Games
                .OrderByDescending(g => g.Date.Date)
                .ProjectTo<GameListingServiceModel>(this.mapper)
                .Take(3)
                .ToList();

        public GameDetailsServiceModel GetDetails(string id)
            => this.data
                .Games
                .Where(g => g.Id == id)
                .ProjectTo<GameDetailsServiceModel>(this.mapper)
                .FirstOrDefault();

        public bool IsByAdmin(string id, int adminId)
            => this.data
                .Games
                .Any(c => c.Id == id && c.AdminId == adminId);

        private static IEnumerable<GameListingServiceModel> GetGames(
            IQueryable<Game> gameQuery,
            IConfigurationProvider mapper)
                => gameQuery
                    .ProjectTo<GameListingServiceModel>(mapper)
                    .ToList();
    }
}
