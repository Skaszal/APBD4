using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!IsFirstAndLastNameValid(firstName, lastName))
            {
                return false;
            }

            if (!IsEmailValid(email))
            {
                return false;
            }

            
            var age = CalculateAge(dateOfBirth);

            if (IsAgeValid(age))
            {
                return false;
            }

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            SetCreditLimitFromType(client, user);

            if (!IsUserUnderCreditLimit(user))
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        private static bool IsUserUnderCreditLimit(User user)
        {
            return !(user.HasCreditLimit && user.CreditLimit < 500);
        }

        private static void SetCreditLimitFromType(Client client, User user)
        {
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }
        }

        private static bool IsAgeValid(int age)
        {
            return age < 21;
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;
            return age;
        }

        private static bool IsEmailValid(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        private static bool IsFirstAndLastNameValid(string firstName, string lastName)
        {
            return !(string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName));
        }
    }
}
