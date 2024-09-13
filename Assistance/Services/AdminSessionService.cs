using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistance.Services
{
    public class AdminSessionService
    {
        private bool _isLoggedIn;
        private string _adminName;

        // Propiedad para verificar si el admin inicio sesion
        public bool IsLoggedIn => _isLoggedIn;
        // Metodo para iniciar sesion
        public void Login(string adminName)
        {
            _isLoggedIn = true;
            _adminName = adminName;
        }
        // Metodo para cerrar sesion]
        public void Logout()
        {
            _isLoggedIn = false;
            _adminName = null;
        }
        // Metodo para obtener el nombre del admin
        public string GetAdminName()
        {
            return _isLoggedIn ? _adminName : "No está autenticado";
        }
    }
}
