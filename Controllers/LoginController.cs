using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;

using LxTools.Data;
using LxTools.Sess;


namespace MiniPki.Controllers;

public class LoginController : Controller
{
    private readonly LxSQLServer dbMini;


    public LoginController() //MyData _db)
    {
        dbMini = new LxSQLServer( "mssqlConn" );
    }

    public IActionResult Index()
    {
        ViewBag.Msg = HttpContext.Session.GetString("Msg");
        HttpContext.Session.Remove("Msg");
        return View();
    }

    public IActionResult End()
    {
        HttpContext.Session.Clear();
        //bool esDispositivoMovil = HttpContext.Request.b .Current.Request.Browser.IsMobileDevice;      
        //string? userAgent = Request.Headers["User-Agent"];
        //Console.WriteLine( userAgent );
        return Redirect( "/" );
    }

    [HttpPost]
    public IActionResult Start(string user, string pass, int[] valor)
    {
        /*
        string consultaSql = "SELECT * FROM MiTabla WHERE Algo = @Algo";
        SqlParameter parametroAlgo = new SqlParameter("@Algo", "valor");
        var resultado = db.Database.ExecuteSqlCommand(consultaSql, parametroAlgo);
        */

        // anti XSS
        user = LxSess.SecureStr(user);
        pass = LxSess.SecureStr(pass);

        var usuario = dbMini.GetRow( "Usuarios", new{Usuario=user, Clave=pass} );
        if (usuario == null)
        {
            HttpContext.Session.SetString( "Msg", "Datos no registrados o errados" );
            return RedirectToAction("Index", "Login"); //, new { Msg = "No coinciden los datos" });
        }

        // seteamos los datos de session, leidos en PanelController
        //
        HttpContext.Session.Set<SessData>("tmpSess", new SessData
        {
            Id        = usuario.Id,
            IdTipo    = usuario.IdTipo,
            UserName  = usuario.Usuario,
            UserAlias = usuario.Responsable
        } );

        // return RedirectToAction("OtraAccion", new { id = 123 });
        return RedirectToAction( "Index", "Panel" );
    }
}
