namespace Poplike.Common.Validation;

public static class Pattern
{
    public static class Common
    {
        public const string Anything = @"^.*$";
        public const string AnythingMultiLine = @"^(\n|\r|.)*$";
        public const string SomeContent = @"^.*[\S].*$";

        public static class Email
        {
            public const string Address =
                @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        }

        public static class Link
        {
            public const string Url = @"^https?://.+$";
        }

        public static class Locale
        {
            public const string Culture =
                @"^[a-z]{2,3}(-[a-zA-Z]{2,3})?$";
        }

        public static class Number
        {
            public const string PriceNoDecimals = @"^[\d\s]*\d[\d\s]*$";
            public const string PriceWithTwoDecimalsPositive = @"^[\d\s]*\d(,\d{0,2})?$";
        }

        public static class Phone
        {
            public const string Number = @"^+[-\d\s]+$";
        }

        public static class Ssn
        {
            public const string Ssn10 = @"^\d{10}$";
        }

        public static class User
        {
            public const string Name = Common.SomeContent;
        }
    }
}
