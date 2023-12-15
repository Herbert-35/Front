using System.Security;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using FrontAPI.Data;
using FrontAPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FrontAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;


        public SeedController(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment env,
            IConfiguration configuration)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _env = env;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult> CreateDefaultUsers()
        {
            // setup the default role names
            string role_RegisteredUser = "RegisteredUser";
            string role_Administrator = "Administrator";
            // create the default roles (if they don't exist yet)
            if (await _roleManager.FindByNameAsync(role_RegisteredUser) ==
            null)
                await _roleManager.CreateAsync(new
                IdentityRole(role_RegisteredUser));
            if (await _roleManager.FindByNameAsync(role_Administrator) ==
            null)
                await _roleManager.CreateAsync(new
                IdentityRole(role_Administrator));
            // create a list to track the newly added users
            var addedUserList = new List<ApplicationUser>();
            // check if the admin user already exists
            var email_Admin = "admin@email.com";
            if (await _userManager.FindByNameAsync(email_Admin) == null)
            {
                // create a new admin ApplicationUser account
                var user_Admin = new ApplicationUser()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = email_Admin,
                    Email = email_Admin,
                };

                // insert the admin user into the DB
                await _userManager.CreateAsync(user_Admin, _configuration["DefaultPasswords:Administrator"]);
                // assign the "RegisteredUser" and "Administrator" roles
                await _userManager.AddToRoleAsync(user_Admin,
                role_RegisteredUser);
                await _userManager.AddToRoleAsync(user_Admin,
                role_Administrator);
                // confirm the e-mail and remove lockout
                user_Admin.EmailConfirmed = true;
                user_Admin.LockoutEnabled = false;
                // add the admin user to the added users list
                addedUserList.Add(user_Admin);
            }
            // check if the standard user already exists
            var email_User = "user@email.com";
            if (await _userManager.FindByNameAsync(email_User) == null)
            {
                // create a new standard ApplicationUser account
                var user_User = new ApplicationUser()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = email_User,
                    Email = email_User
                };
                // insert the standard user into the DB
                await _userManager.CreateAsync(user_User, _configuration["DefaultPasswords:RegisteredUser"]);
                // assign the "RegisteredUser" role
                await _userManager.AddToRoleAsync(user_User,
                role_RegisteredUser);
                // confirm the e-mail and remove lockout
                user_User.EmailConfirmed = true;
                user_User.LockoutEnabled = false;

                // add the standard user to the added users list
                addedUserList.Add(user_User);
            }
            // if we added at least one user, persist the changes into the DB
            if (addedUserList.Count > 0)
                await _context.SaveChangesAsync();
            return new JsonResult(new
            {
                Count = addedUserList.Count,
                Users = addedUserList
            });
        }


        [HttpGet]
        public async Task<ActionResult> Import()
        {
            // prevents non-dev enviorments from runing this method
            if (!_env.IsDevelopment())
            { throw new SecurityException("Not allowed"); }

            var path = Path.Combine(_env.ContentRootPath, "Data/Source/Restaurant.xlsx");

            using var stream = System.IO.File.OpenRead(path);
            using var excelPackage = new ExcelPackage(stream);

            //get the first WS
            var worksheet = excelPackage.Workbook.Worksheets[0];

            //define how many rows we want to process
            var nEndRow = worksheet.Dimension.End.Row;

            //initialize the record counters
            var numberOfStatesAdded = 0;
            var numberOfLocationsAdded = 0;

            //create a Lokup Directory
            //containing all the states already existing
            //into the Database (it wil empty on first run
            var statesByName = _context.States
                .AsNoTracking()
                .ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

            //iterates through all rows skiping the first one
            for (int nRow = 2; nRow <= nEndRow; nRow++)
            {
                var row = worksheet.Cells[nRow, 1, nRow, worksheet.Dimension.End.Column];

                var restaurantName = row[nRow, 1].GetValue<string>();
                var phoneNumber = row[nRow, 2].GetValue<double>();
                var stateName = row[nRow, 3].GetValue<string>();
                var cuisineName = row[nRow, 7].GetValue<string>();

                //skip thie state if it already exists in the database
                if (statesByName.ContainsKey(stateName))
                { continue; }

                //create the State entity and fill it with xlsc data
                var state = new State
                {
                    Name = stateName,
                    RestaurantName = restaurantName,
                    PhoneNumber = phoneNumber,
                    Cuisine = cuisineName,
                };

                //add the new state to the DB context
                await _context.States.AddAsync(state);

                //store the state in our lokup to retrieve its ID later on
                statesByName.Add(stateName, state);

                //increment the counter
                numberOfStatesAdded++;
            }

            //save all the states into the Database
            if (numberOfStatesAdded > 0)
            { await _context.SaveChangesAsync(); }

            //create a lokup dictionary
            //containing all the locations already existing
            //into the DB (it will be empty on the first run)

            var locations = _context.Locations
                .AsNoTracking()
                .ToDictionary(x => (
                    Name: x.Name,
                    ZipCode: x.ZipCode,
                    StreetAddress: x.StreetAddress,
                    StateId: x.StateId));

            //iterates through all rows, skipping the first one 
            for (int nRow = 2; nRow <= nEndRow; nRow++)
            {
                var row = worksheet.Cells[nRow, 1, nRow, worksheet.Dimension.End.Column];
                var stateName = row[nRow, 3].GetValue<string>();
                var name = row[nRow, 4].GetValue<string>();
                var zipCode = row[nRow, 5].GetValue<int>();
                var streetAddress = row[nRow, 6].GetValue<string>();

                //retrieve 
                var stateId = statesByName[stateName].Id;
                //skip this location if it alrady exists in the DB
                if (locations.ContainsKey((
                    Name: name,
                    ZipCode: zipCode,
                    StreetAddress: streetAddress,
                    StateId: stateId)))
                    continue;

                //create the location entity and fill it with xlsx data
                var location = new Location
                {
                    Name = name,
                    ZipCode = zipCode,
                    StreetAddress = streetAddress,
                    StateId = stateId
                };

                //add the new location to the DB context
                _context.Locations.Add(location);

                //increment the counter 
                numberOfLocationsAdded++;
            }

            //save all the locations into the DB
            if (numberOfLocationsAdded > 0)
            { await _context.SaveChangesAsync(); }

            return new JsonResult(new
            {
                Locations = numberOfLocationsAdded,
                States = numberOfStatesAdded
            });
        }
    }
}
