using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GitBorAl
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string Email;
            string Password;
            string Date;
            string Phone;
            CheckFields checkFields = new CheckFields();
            do
            {
                Console.Write("Введите email: ");
                Email = (Console.ReadLine());
                Console.Write("Введите дату: ");
                Date = (Console.ReadLine());
                Console.Write("Введите пароль: ");
                Password = (Console.ReadLine());
                Console.Write("Введите номер телефона: ");
                Phone = (Console.ReadLine());
            } while (checkFields.IsEmailValid(Email) == false && checkFields.IsDateValid(Date) == false && checkFields.IsPasswordValid(Password) == false
            && checkFields.IsPhoneValid(Phone) == false);
            Console.WriteLine("Прошло успешно! ");
            Console.ReadKey();
        }


            public abstract class CheckFields : Validator
        {
            private const int kMinimumLength = 8;
            private static string _specialChars = "@#_&!?:;%$^";
            private static bool IsSpecialChar(char c) { return _specialChars.IndexOf(c) >= 0; }
            private static bool IsValidPasswordChar(char c) { return IsSpecialChar(c) || Char.IsLetterOrDigit(c); }
            public override bool IsPasswordValid(string password)
            {
                if (password == null || password.Length < kMinimumLength)
                    return false;
                bool hasLetter = false, hasDigit = false;
                int specials = 0;
                foreach (char c in password)
                {
                    hasDigit = hasDigit || Char.IsDigit(c);
                    hasLetter = hasLetter || Char.IsLetter(c);
                    specials += IsSpecialChar(c) ? 1 : 0;
                    if (!IsValidPasswordChar(c)) return false;
                }
                return hasDigit && hasLetter && specials > 1;
            }
            public override string HashPassword(string password)
            {
                var Md5 = MD5.Create();
                var hash = Md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
            public abstract bool IsUserExists(string login, string password);
            public override bool IsEmailValid(string email)
            {
                if (new EmailAddressAttribute().IsValid(email) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override bool IsPhoneValid(string phone)
            {
                return Regex.Match(phone, @"^(\+[0-9]{9})$").Success;
            }
            public abstract bool IsWebPageAvailable(string url);
            public abstract bool IsDatabaseAccessible(string connectionString);
            public override bool IsDateValid(string date)
            {
                DateTime ValidDate;
                if (DateTime.TryParse(date, out ValidDate) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public abstract bool IsUserRoot();
            public abstract void Log();
            
        }
    }
}
