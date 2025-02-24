﻿namespace MiniFootball
{
    public static class GlobalConstants
    {
        public const string DefaultConnection = "DefaultConnection";
        
        public const int CookieTimeOut = 5;

        public const string LatestGamesCacheKey = "AllCountriesCacheKey";

        public const string Create = "Create";
        
        public const string SomethingIsWrong = "Something is wrong. Try again!";

        public static class Error
        {
            public const string Name = "Error";

            public const string ErrorPage = "/Error/{0}";

            public const string ErrorRoute = "Error/{statusCode}";

            public const string ErrorMessage404 = "Sorry, the resource you requested could not be found!";
        }

        public static class Notifications
        {
            public const int DurationInSeconds = 4;
            
            public const int DefaultDurationInSeconds = 10;

            public const string GreenColor = "#16ff00";
            
            public const string Welcome = "Welcome to Mini Football!";

            public const string Goodbye = "Goodbye!";

            public const string InvalidEmailOrPassword = "Invalid email or password!";
        }

        public static class AccountsSeeding
        {
            public const string Password = "123456";

            public const string AdminEmail = "admin@admin.com";

            public const string ManagerEmail = "manager@manager.com";

            public const string UserEmail = "user@user.com";
        }

        public static class Api
        {
            public const string ApiStatisticsRoute = "api/statistics";
        }

        public static class GameUser
        {
            public const string UserDoesNotExist = "User does not exist!";
        }

        public static class Login
        {
            public const string RememberMe = "Remember Me?";
        }

        public static class Admin
        {
            public const string AreaName = "Admin";
            
            public const string ControllerName = "Admins";
            
            public const string SuccessfullyBecome = "You successfully have become an admin!";
            
            public const string CanNotBecomeAdmin = "You can not become an admin!";
        }

        public static class City
        {
            public const string ControllerName = "Cities";
            
            public const string OnlyAdminCanCreate = "Only Admins can create city!";
            
            public const string ThereAreAlreadyACity = "There are already a City with this Name and Country!";
            
            public const string SuccessfullyCreated = "You successfully created city!";
            
            public const string CityDoesNotExistInCountry = 
                "This City does not exist in this Country. First you have to create City and then create Game!";
        }

        public static class Field
        {
            public const string ControllerName = "Fields";
         
            public const string SuccessfullyCreated = "Your field was created and is awaiting approval!";
            
            public const string SuccessfullyDelete = "You successfully deleted field!";
            
            public const string DoesNotExist = "Field does not exist. Try again!";
            
            public const string IncorrectParameters = "Field parameters are incorrect. Try again!";

            public const string OnlyAdminCanCreate = "Only Admins can create fields!";
            
            public const string OnlyCreatorCanEdit = 
                "Only the creator of the field or moderator can edit the field!";
            
            public const string YouDoNotHaveAnyFieldsYet = 
                "You do not have any fields yet, but you can create them!";

            public const string DoNotHaveAnyFieldsYet =
                "There are no fields, but you can create one!";

            public const string OnlyCreatorCanDelete = 
                "Only the creator of this field and Moderator can delete the field!";
            
            public const string ThereAreAlreadyExistField = 
                "There are already exist fields with this Name, Country and City";

            public const string ThereAreNoFieldsInThisCity =
                "There are no fields in this city. First Create Field in this City and then Create Game!";
        }

        public static class Game
        {
            public const string Mine = "Mine";
            
            public const string ControllerName = "Games";
            
            public const string UserExited = "User exited from the game!";
            
            public const string DoesNotExist = "Game Does Not Exist!";

            public const string IncorrectDate = "The date has to be today or after today!";
            
            public const string YouCanNotRemovePlayer = "You can not remove player from this game!";

            public const string CreateGameChooseField = "CreateGameChooseField";
            
            public const string CreateGameLastStep = "CreateGameLastStep";
            
            public const string YetNoPlayers = "There are no players for this game yet!";

            public const string OnlyAdminCanCreate = "Only Admins can create game!";
            
            public const string OnlyJoinedPlayersCanView = "Only joined players can view the list of players!";

            public const string CreateGameFirstStep = "CreateGameFirstStep";
            
            public const string SuccessfullyCreated = "Your game was created and is awaiting approval!";
            
            public const string SuccessfullyDeleted = "You successfully deleted game!";
            
            public const string SuccessfullyJoined = "You successfully joined game!";

            public const string YouDoNotHaveAnyGamesYet =
                "You do not have any games yet, but you can create one!";

            public const string DoNotHaveAnyGamesYet =
                "There are no games, but you can create one!";

            public const string ThereAreAlreadyAGame =
                "There are already a game in this field in this date and time! Choose another time!";

            public const string OnlyCreatorCanEdit =
                "Only the creator of the game or moderator can edit the game!";

            public const string OnlyCreatorCanDelete =
                "Only the creator of this game and Moderator can delete the game!";
        }

        public static class Manager
        {
            public const string AreaName = "Manager";
            
            public const string ManagerRoleName = "Manager";
        }

        public static class Home
        {
            public const string ControllerName = "Home";
            
            public const string Error = "Error";
            
            public const string HomePage = "/";
        }
    }
}
