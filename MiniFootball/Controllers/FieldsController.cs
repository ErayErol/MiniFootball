﻿namespace MiniFootball.Controllers
{
    using AutoMapper;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Fields;
    using Services.Admins;
    using Services.Cities;
    using Services.Countries;
    using Services.Fields;
    using System.Linq;
    using Areas.Admin.Controllers;
    using static Convert;
    using static WebConstants;
    using static GlobalConstant;

    public class FieldsController : Controller
    {
        private readonly ICountryService countries;
        private readonly ICityService cities;
        private readonly IAdminService admins;
        private readonly IFieldService fields;
        private readonly IMapper mapper;

        public FieldsController(
            ICountryService countries,
            IFieldService fields,
            ICityService cities,
            IMapper mapper,
            IAdminService admins)
        {
            this.countries = countries;
            this.fields = fields;
            this.cities = cities;
            this.mapper = mapper;
            this.admins = admins;
        }

        public IActionResult All([FromQuery] FieldAllQueryModel query)
        {
            var queryResult = fields.All(
                query.City,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.PlaygroundsPerPage);

            var queryCities = fields.AllCreatedCitiesName();

            query.TotalFields = queryResult.TotalFields;
            query.Fields = queryResult.Fields;
            query.Cities = queryCities;

            return View(query);
        }

        [Authorize]
        public IActionResult Create()
        {
            if (admins.IsAdmin(User.Id()) == false || User.IsManager())
            {
                TempData[GlobalMessageKey] = Field.OnlyAdminCanCreate;
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            return View(new FieldCreateFormModel
            {
                Countries = countries.All(),
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(FieldCreateFormModel fieldModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 || User.IsManager())
            {
                TempData[GlobalMessageKey] = Field.OnlyAdminCanCreate;
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            if (ModelState.IsValid == false)
            {
                fieldModel.Countries = countries.All();
                return View(fieldModel);
            }

            fieldModel.CountryId = countries.CountryIdByName(fieldModel.CountryName);
            fieldModel.CityId = cities.CityIdByName(fieldModel.CityName);

            if (fieldModel.CityId == 0)
            {
                TempData[GlobalMessageKey] = City.CityDoesNotExistInCountry;
                return RedirectToAction(GlobalConstant.Create, City.ControllerName);
            }

            if (fields.IsAlreadyExist(fieldModel.Name, fieldModel.CountryId, fieldModel.CityId))
            {
                fieldModel.Countries = countries.All();
                TempData[GlobalMessageKey] = Field.ThereAreAlreadyExistField;
                return View(fieldModel);
            }

            fieldModel.CountryName = ToTitleCase(fieldModel.CountryName);
            fieldModel.CityName = ToTitleCase(fieldModel.CityName);

            var fieldId = fields.Create(
                fieldModel.Name,
                fieldModel.CountryId,
                fieldModel.CityId,
                fieldModel.Address,
                fieldModel.ImageUrl,
                fieldModel.PhoneNumber,
                fieldModel.Parking,
                fieldModel.Cafe,
                fieldModel.Shower,
                fieldModel.ChangingRoom,
                fieldModel.Description,
                adminId);

            if (fieldId == 0)
            {
                TempData[GlobalMessageKey] = SomethingIsWrong;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            TempData[GlobalMessageKey] = Field.SuccessfullyCreated;
            return RedirectToAction(nameof(Mine));
        }

        [Authorize]
        public IActionResult Edit(int id, string information)
        {
            var userId = User.Id();

            if (admins.IsAdmin(userId) == false && User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = Field.OnlyCreatorCanEdit;
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            var fieldDetails = fields.GetDetails(id);

            if (fieldDetails.Name.Equals(information) == false)
            {
                TempData[GlobalMessageKey] = SomethingIsWrong;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            var fieldEdit = mapper.Map<FieldEditFormModel>(fieldDetails);

            return View(fieldEdit);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(FieldEditFormModel fieldModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 && User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = Field.OnlyCreatorCanEdit;
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            if (ModelState.IsValid == false)
            {
                return View(fieldModel);
            }

            if (fields.IsAdminCreatorOfField(fieldModel.Id, adminId) == false && User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = Field.OnlyCreatorCanEdit;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            var isEdit = fields.Edit(
                fieldModel.Id,
                fieldModel.Name,
                fieldModel.Address,
                fieldModel.ImageUrl,
                fieldModel.Parking,
                fieldModel.Shower,
                fieldModel.ChangingRoom,
                fieldModel.Cafe,
                fieldModel.Description,
                fieldModel.PhoneNumber,
                User.IsManager());

            if (isEdit == false)
            {
                TempData[GlobalMessageKey] = SomethingIsWrong;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            var fieldName = fields.FieldName(fieldModel.Id);

            TempData[GlobalMessageKey] =
                $"Your field was edited{(User.IsManager() ? string.Empty : " and is awaiting approval")}!";
            return RedirectToAction(nameof(Details), new { id = fieldModel.Id, information = fieldName });
        }

        [Authorize]
        public IActionResult Delete(int id, string information)
        {
            var userId = User.Id();

            if (admins.IsAdmin(userId) == false && User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = Field.OnlyCreatorCanDelete;
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            var field = fields.FieldExist(id);

            if (field == false)
            {
                TempData[GlobalMessageKey] = Field.DoesNotExist;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            var fieldDeleteDetails = fields.FieldDeleteInfo(id);

            if (fieldDeleteDetails.Name.Equals(information) == false)
            {
                TempData[GlobalMessageKey] = SomethingIsWrong;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            return View(fieldDeleteDetails);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(FieldDeleteServiceModel fieldModel)
        {
            var adminId = admins.IdByUser(User.Id());

            if (adminId == 0 && User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = Field.OnlyCreatorCanDelete;
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            if (fields.IsAdminCreatorOfField(fieldModel.Id, adminId) == false && User.IsManager() == false)
            {
                TempData[GlobalMessageKey] = Field.OnlyCreatorCanDelete;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            if (fields.Delete(fieldModel.Id) == false)
            {
                TempData[GlobalMessageKey] = SomethingIsWrong;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            TempData[GlobalMessageKey] = Field.SuccessfullyDelete;
            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Details(int id, string information)
        {
            var fieldDetails = fields.GetDetails(id);

            if (fieldDetails == null || fieldDetails.Name.Equals(information) == false)
            {
                TempData[GlobalMessageKey] = SomethingIsWrong;
                return RedirectToAction(Home.Error, Home.ControllerName);
            }

            return View(fieldDetails);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var userId = User.Id();

            if (admins.IsAdmin(userId) == false || User.IsManager())
            {
                return RedirectToAction(nameof(AdminsController.Become), Admin.ControllerName);
            }

            var myFields = fields
                .FieldsWhereCreatorIsUser(userId)
                .OrderByDescending(f => f.Id);

            if (myFields.Any() == false)
            {
                TempData[GlobalMessageKey] = Field.YouDoNotHaveAnyFieldsYet;
            }

            return View(myFields);
        }
    }
}
