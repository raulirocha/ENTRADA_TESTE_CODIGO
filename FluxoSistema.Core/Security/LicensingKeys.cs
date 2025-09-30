
namespace FluxoSistema.Core.Security
{
    public static class LicensingKeys
    {
        // Este é o "verificador de autenticidade" que seu ERP usará.
        // Ele não é secreto e serve apenas para validar as licenças geradas pelo portal.
        public const string PublicKeyXml = @"<RSAKeyValue><Modulus>x6eUD0fj7xC2UAKNmF90apKj4fa5JC2LwQ3T38jtUVw7omTAue1Z3GIzGZL5vZeAoZ/NU63iQmUzvACZgslzW9BO8OZGGQAhoKGhN2wlJ7zyLUB4lBIQJub56ie/x9TFqgQtR3IE5rCeE4MIjEiHdijdKkOg3qOUbZFVzfqEuT5IipAbJBMJ2datpw+GcXjOy4kVxAui7Du8z4i6vrc2rGZmD6WdvKcxq4KYyk/1Y8dLkIzazlp34LTu6hAg9TKmvRFyXxMut0ubY8QO2ajH3sdKNfLG6tT/pIt3QhSnA44H45ff2tWL+g42BEM0LdrKkTr29nP6OLE0c+EhbSR1MQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
    }
}