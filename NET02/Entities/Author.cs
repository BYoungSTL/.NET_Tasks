using System;

namespace NET02.Entities
{
    public class Author 
    {
        private const int NameLength = 200;
        private readonly string _firstName;
        private readonly string _lastName;

        private static bool IsCorrectValue(string value) => value.Length > NameLength && !string.IsNullOrEmpty(value); 
        public string FirstName
        {
            get => _firstName;
            private init
            {
                if (IsCorrectValue(value)) 
                {
                    throw new ArgumentException("Invalid First name of Author");
                }

                _firstName = value;
            }
        }
        
        public string LastName
        {
            get => _lastName;
            private init
            {
                if (IsCorrectValue(value))
                {
                    throw new ArgumentException("Invalid Last name of Author");
                }

                _lastName = value;
            }
        }

        public Author(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Author)
            {
                return false;
            }
            Author s = obj as Author;
            return FirstName == s?.FirstName && LastName == s?.LastName;
        }
    }
}