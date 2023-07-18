using System.Runtime.CompilerServices;

namespace Poplike.Common.Validation;

public static class MaxLengths
{
    public static class Common
    {
        // Maximum lengths are used both to define the database
        // table column widths, as well as to validate user input.

        public const int DbString = 4000;

        // A fallback max length. There's a soft limit of 4000 UTF16 chars
        // (8000 bytes) in MS SQL Server, for column type nvarchar, 
        // after which data storage fans out to additional pages.

        public static class Blurb
        {
            public const int Text = 512;
        }

        public static class Email
        {
            public const int Address = 128;
        }

        public static class Link
        {
            public const int Url = 200;
            public const int Description = 100;
        }

        public static class Person
        {
            public const int Name = 40;
        }

        public static class Phone
        {
            public const int Number = 20;
        }

        public static class Postal
        {
            public const int Address = 50;
            public const int ZipCode = 10;
            public const int City = 25;
        }

        public static class Emoji
        {
            public const int Characters = 20;
            // NOTE: To fit a single emoji.
            // (Even the more complex ones?)
        }

        public static class Url
        {
            public const int Homepage = 200;
        }
    }

    public static class Domain
    {
        public static class Expression
        {
            public const int Characters = 20;
        }

        public static class EmailOut
        {
            public const int ToName = Common.Person.Name;
            public const int ToAddress = Common.Email.Address;

            public const int FromName = Common.Person.Name;
            public const int FromAddress = Common.Email.Address;

            public const int ReplyToName = Common.Person.Name;
            public const int ReplyToAddress = Common.Email.Address;
            
            public const int Subject = 300;

            public const int HtmlBody = Common.DbString;
            public const int TextBody = Common.DbString;
        }

        public static class Keyword
        {
            public const int Word = 30;
        }

        public static class Language
        {
            public const int Name = 30;
            public const int Culture = 10;
            public const int Emoji = Common.Emoji.Characters;
        }

        public static class LogEntry
        {
            public const int Message = 512;
        }

        public static class ExpressionSet
        {
            public const int Emoji = Common.Emoji.Characters;
            public const int Name = 40;
        }

        public static class Rule
        {
            public const int Heading = 80;
            public const int Text = 255;
        }

        public static class Statement
        {
            public const int Sentence = 100;
        }

        public static class Subject
        {
            public const int Name = 60;
        }

        public static class Category
        {
            public const int Emoji = Common.Emoji.Characters;
            public const int Name = 50;

            public const int SubjectHeading = 50;
            public const int SubjectPlaceholder = 50;
        }

        public static class User
        {
            public const int Name = Common.Person.Name;
            public const int Password = 128;
        }

        public static class Word
        {
            public const int Value = 40;
        }
    }
}
