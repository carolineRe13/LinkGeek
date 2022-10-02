#nullable disable


using LinkGeek.AppIdentity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinkGeek.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        public List<ItemList> GenderList { get; set; }  = new() {
            new ItemList { Text = "Female", Value = 1 },  
            new ItemList { Text = "Male", Value = 2 },  
            new ItemList { Text = "Prefer not to say", Value = 3 }  
        };
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Username")]
            public string Username { get; set; }
            
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            
            [Display(Name = "Gender")]
            public string Gender { get; set; }
            
            [Display(Name = "Profile picture")]
            [BindProperty]
            public IFormFile ProfilePicture { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var gender = await _userManager.GetGender(user);

            Input = new InputModel
            {
                Username = userName,
                PhoneNumber = phoneNumber,
                Gender = gender
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userName = await _userManager.GetUserNameAsync(user);
            if (Input.Username != userName)
            {
                var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.Username);
                await UpdateSuccessful(user, "user name");
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                await UpdateSuccessful(user, "phone number");
            }
            
            
            if (Input.Gender != user.Gender)
            {
                user.Gender = Input.Gender;
                await UpdateSuccessful(user, "gender");
            }
            
            if (Input.ProfilePicture != null) {
                using (var ms = new MemoryStream())
                {
                    await Input.ProfilePicture.CopyToAsync(ms);
                    if (ms.Length > 0 ) {
                        user.ProfilePictureContentType = Input.ProfilePicture.ContentType;
                        user.ProfilePictureData = Convert.ToBase64String(ms.ToArray()); 
                        await UpdateSuccessful(user, "profile picture");
                    }
                    else
                    {
                        StatusMessage = "Unexpected error when trying to set profile picture";
                    }
                }
            
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage ??= "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task UpdateSuccessful(ApplicationUser user, string updatedField)
        {
            var update = await _userManager.UpdateAsync(user);
            if (!update.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set " + updatedField;
            }
        }
    }
}
