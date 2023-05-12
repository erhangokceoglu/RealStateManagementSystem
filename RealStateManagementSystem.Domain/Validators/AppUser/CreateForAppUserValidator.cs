using FluentValidation;
using RealStateManagementSystem.Domain.Dtos.AppUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Domain.Validators.AppUser
{
    public class CreateForAppUserValidator : AbstractValidator<CreateForAppUserDto>
    {
        public CreateForAppUserValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NotNull().WithMessage("Lütfen isim alanını boş geçmeyiniz.")
            .MaximumLength(100).MinimumLength(5).WithMessage("Lütfen isim alanını 5 ile 100 karakter arasında giriniz.");

            RuleFor(p => p.Surname).NotEmpty().NotNull().WithMessage("Lütfen soyad alanını boş geçmeyiniz.")
            .MaximumLength(100).MinimumLength(5).WithMessage("Lütfen soyad alanını 5 ile 100 karakter arasında giriniz.");

            RuleFor(p => p.Email).NotEmpty().NotNull().WithMessage("Lütfen email alanını boş geçmeyiniz.")
            .MaximumLength(100).MinimumLength(5).WithMessage("Lütfen email alanını 10 ile 100 karakter arasında giriniz.");

            RuleFor(p => p.Password).NotEmpty().NotNull().WithMessage("Lütfen şifre alanını boş geçmeyiniz.")
            .MaximumLength(100).MinimumLength(5).WithMessage("Lütfen şifre alanını 8 ile 100 karakter arasında giriniz.");

            RuleFor(p => p.Address).NotEmpty().NotNull().WithMessage("Lütfen adres alanını boş geçmeyiniz.")
            .MaximumLength(180).MinimumLength(5).WithMessage("Lütfen adres alanını 5 ile 180 karakter arasında giriniz.");
        }
    }
}
