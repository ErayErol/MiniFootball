﻿namespace MessiFinder.Data
{
    public class DataConstants
    {
        public class ErrorMessages
        {
            public const string Empty = "Cannot be empty!";
            
            public const string Range = "Must be in range {2} and {1}";
        }

        public class Cloudinary
        {
            public const string CloudName = "messi-finder";
            
            public const string ApiKey = "585982319676539";
            
            public const string ApiSecret = "Rv-7T5OK75aBH7ie-IpW7ibklSY";
            
            public const string APIEnvironmentVariable = "CLOUDINARY_URL=cloudinary://585982319676539.Rv-7T5OK75aBH7ie-IpW7ibklSY@messi-finder";
        }

        public class Game
        {
            public const int NumberOfPlayersMin = 8;
            public const int NumberOfPlayersMax = 22;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 200;
        }

        public class Playground
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 26;

            public const int CountryMinLength = 1;
            public const int CountryMaxLength = 56;

            public const int TownMinLength = 1;
            public const int TownMaxLength = 85;

            public const int AddressMinLength = 1;
            public const int AddressMaxLength = 100;

            public const int PhoneNumberMinLength = 8;
            public const int PhoneNumberMaxLength = 15;

            public const int DescriptionMinLength = 50;
            public const int DescriptionMaxLength = 500;
        }

        public class Admin
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 26;

            public const int PhoneNumberMinLength = 8;
            public const int PhoneNumberMaxLength = 15;
        }

        public class Player
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 26;
        }
    }
}
