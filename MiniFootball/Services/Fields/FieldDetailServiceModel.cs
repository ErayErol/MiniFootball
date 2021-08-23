﻿namespace MiniFootball.Services.Fields
{
    using Models.Fields;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants;
    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Field;

    public class FieldDetailServiceModel : FieldEditFormModel
    {
        [StringLength(CountryMaxLength, MinimumLength = CountryMinLength, ErrorMessage = Range)]
        [Display(Name = Country.CountryName)]
        public string CountryName { get; set; }

        [StringLength(CityMaxLength, MinimumLength = CityMinLength, ErrorMessage = Range)]
        [Display(Name = City.CityName)]
        public string CityName { get; set; }
    }
}