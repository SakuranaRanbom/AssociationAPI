using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TeamAPI.Models
{
    public class PassValidator<TUser> : IPasswordValidator<TUser> where TUser : class
    {
        public PassValidator(IdentityErrorDescriber errors = null) => this.Describer = errors ?? new IdentityErrorDescriber();
        public IdentityErrorDescriber Describer { get; private set; }
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof (password));
            if (manager == null)
                throw new ArgumentNullException(nameof (manager));
            List<IdentityError> identityErrorList = new List<IdentityError>();
            PasswordOptions password1 = manager.Options.Password;
            if (string.IsNullOrWhiteSpace(password) || password.Length < password1.RequiredLength)
                identityErrorList.Add(this.Describer.PasswordTooShort(password1.RequiredLength));
            return Task.FromResult<IdentityResult>(identityErrorList.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(identityErrorList.ToArray()));
        }
    }
}