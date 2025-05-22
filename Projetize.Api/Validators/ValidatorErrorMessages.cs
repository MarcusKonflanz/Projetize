namespace Projetize.Api.Validators
{
    public class ValidatorErrorMessages
    {
        //Cadastro Usuário
        public static string NonEmptyName = "O campo nome não pode ser vázio!";
        public static string NameHighestLimit = "O nome ultrapassa o limite de caracteres";

        public static string NonEmptyEmail = "O campo e-mail não pode ser vázio!";
        public static string EmailHighestLimit = "O e-mail ultrapassa o limite de caracteres";

        public static string NonEmptyUserName = "O campo nome de usuário não pode ser vázio!";
        public static string UserNameHighestLimit = "O campo nome de usuário o limite de caracteres";

        public static string NonEmptyPassword = "O campo senha não pode ser vázio!";
        public static string PasswordHighestLimit = "A senha ultrapassa o limite de 15 caracteres";
        public static string PasswordLowerLimit = "A senha exige ao menos 8 caracteres";
        public static string NonUpperCaseLetter = "A senha deve conter ao menos uma letra minuscula";
        public static string NonLowerCaseLetter = "A senha deve conter ao menos uma letra maiuscula";
        public static string NonLeastOneNumber = "A senha deve conter pelo menos um número";
        public static string NonLeastOneSpecialCharacter = "A senha deve conter ao menos um caractere especial";

        //Login
        public static string NonEmptyLogin = "O campo de login não pode ser vázio.";

    }
}
