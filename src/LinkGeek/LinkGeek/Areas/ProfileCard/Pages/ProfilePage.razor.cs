using LinkGeek.AppIdentity;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace LinkGeek.Areas.ProfileCard.Pages;

public class FormModel
{
    [Required]
    [StringLength(256, ErrorMessage = "Too long.")]
    public string? Value { get; set; }
}

/// <summary>
/// Class <c>ProfilePage</c> a user's profile page. This can be the current user or someone else's profile
/// </summary>
[Authorize]
public partial class ProfilePage
{
    [Inject] private UserService _userService { get; set; }

    [Inject] private UserManager<ApplicationUser> userManager { get; set; }

    [Inject]
    private ISnackbar Snackbar { get; set; }

    [CascadingParameter] public ApplicationUser? CurrentUser { get; set; }

    [Parameter] public string? UserName { get; set; }

    private ApplicationUser? _displayedUser { get; set; }

    private readonly FormModel _locationForm = new();
    private readonly FormModel _statusForm = new();

    private bool _loading = true;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (this.CurrentUser != null)
        {
            if (DisplayedUserIsCurrentUser())
            {
                this._displayedUser = this.CurrentUser;
                this._locationForm.Value = this.CurrentUser?.Location;
                this._statusForm.Value = this.CurrentUser?.Status;
            }
            else
            {
                this._displayedUser = _userService.GetUserFromUserName(UserName ?? "");
            }

            this._loading = false;
        }
    }

    /// <summary>
    /// Method <c>UpdateLocation</c> updates the location or shows an error in a Snackbar if the method is called by the current user
    /// </summary>
    public async Task UpdateLocation()
    {
        if (CurrentUser == null)
            return;
        
        var result = await _userService.UpdateLocation(CurrentUser, this._locationForm.Value);

        if (result == null)
        {
            Snackbar.Add("Could not update location", Severity.Error);
        }
        else
        {
            CurrentUser.Location = result;
            this.StateHasChanged();
        }
    }

    /// <summary>
    /// Method <c><UpdateStatus/c> updates the status or shows an error in a Snackbar if this is the method is called by the current user
    /// </summary>
    public async Task UpdateStatus()
    {
        if (CurrentUser == null)
            return;

        var result = await _userService.UpdateStatus(CurrentUser, this._statusForm.Value);

        if (result == null)
        {
            Snackbar.Add("Could not update status", Severity.Error);
        }
        else
        {
            CurrentUser.Status = result;
            this.StateHasChanged();
        }
    }

    /// <summary>
    /// Method <c>DisplayedUserIsCurrentUser</c> returns true if the displayed card is the current user
    /// </summary>
    public bool DisplayedUserIsCurrentUser()
    {
        if (UserName == null && CurrentUser != null)
            return true;

        if (CurrentUser != null && CurrentUser.UserName.ToLower() == UserName.ToLower())
            return true;

        return false;
    }
}

