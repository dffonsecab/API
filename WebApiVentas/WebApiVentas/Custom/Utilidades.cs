using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApiVentas.Models;



namespace WebApiVentas.Custom
{
    public class Utilidades
    {
        /* El Iconfiguration permite acceder a las propiedades de archivo appsettings.json */
        private readonly IConfiguration _configuration;



        public Utilidades(IConfiguration configuration)
        {

            _configuration = configuration;


            
         }


        public string encriptarSha256(string texto)
        {
            using (SHA256 sha256Hash=SHA256.Create())
            {

                // Computar el Hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                // Convertir el array de bytes a string
                StringBuilder sb = new StringBuilder();


                for(int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));

                }


                return sb.ToString();

            }
            

        }


        public string generarJWT(Usuario modelo)
        {

            // Crear la informacion del usuario para el token
            /*-- La informacion del usuario se llama desede la clase usuario --*/

            Claim[] userClaims = new []
            {
                new Claim(ClaimTypes.NameIdentifier,modelo.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email,modelo.Correo!)

            };

            // Crear la llave de seguridad

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]!));

            // Creacion de Credenciales

            var Credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


            // Crear detalle del token

            var JwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: Credential

                );


            // Retornar el token

            var Jwtoken = new JwtSecurityTokenHandler().WriteToken(JwtConfig);

            return Jwtoken;

        }



    }
}
