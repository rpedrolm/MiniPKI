using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using LxTools.Data;
using LxTools.Sess;
using System.Web;

namespace MiniPki.Controllers;

public class PanelController : Controller
{
    private readonly LxSQLServer dbMini;

    public PanelController()
    {
        dbMini = new LxSQLServer( "mssqlConn" );
    }

    [HttpGet]
    //[Route("Panel/Ver")]
    public IActionResult Index()
    {
        ViewBag.Session = HttpContext.Session.Get<SessData>("tmpSess");
        if(ViewBag.Session==null) return RedirectToAction( "Index", "Home");
        //------------------------------------------------------------------

        //LxSess.IsMobile( Request.Headers["User-Agent"] );
        //
        if( ViewBag.Session.IdTipo == 1 ) // admin
            ViewBag.Archivos = dbMini.GetTable( "Certificados", new{ 
                    Estado = 1, 
                    Tipo   = 2
                } );
        else
            ViewBag.Archivos = dbMini.GetTable( "Certificados", new{ 
                    Estado = 1, 
                    Tipo   = 2, 
                    IdUser = ViewBag.Session.Id 
                } );

        ViewBag.CAroots = dbMini.GetTable( "Certificados", new{ 
                Estado = 1, 
                Tipo   = 1, 
                IdRoot = 0 
            } );

        return View();
    }

    public IActionResult caroot()
    {
        ViewBag.Session = HttpContext.Session.Get<SessData>("tmpSess");
        if(ViewBag.Session==null) return RedirectToAction( "Index", "Home");
        //------------------------------------------------------------------
        if( ViewBag.Session.IdTipo != 1 ) return Redirect("/Panel");
        //------------------------------------------------------------------

        ViewBag.Archivos = dbMini.GetTable( "Certificados", new{ 
                Estado = 1, 
                Tipo   = 1
            } );

        ViewBag.Usuarios = dbMini.GetTable( "Usuarios", new{ Estado = 1 } );

        return View();
    }

    public IActionResult Tutor()
    {
        ViewBag.Session = HttpContext.Session.Get<SessData>("tmpSess");
        if(ViewBag.Session==null) return RedirectToAction( "Index", "Home");
        //-------------------------------------------------------------------
        return View();
    }

    public IActionResult DoAddCert1( int cboUser, string edtCN, string edtO, string edtC, string edtZ, string email, string Clave )
    {
        SessData? sess = HttpContext.Session.Get<SessData>("tmpSess");
        if(sess==null) return RedirectToAction( "Index", "Home");
        //-------------------------------------------------------------------

        // Cert tipo 2 : de firma digital necesita de un CertRaiz
        //
        edtCN = LxSess.SecureStr( edtCN );
        edtO  = LxSess.SecureStr( edtO );
        edtC  = LxSess.SecureStr( edtC );
        edtZ  = LxSess.SecureStr( edtZ );
        email = LxSess.SecureStr( email );
        //string? inputValue = Request.Form["edtCN"];


        DateTime fechaIni = DateTime.Now;
        DateTime fechaFin = fechaIni.AddMonths(3); // Agregar tres meses

        
        var Id = dbMini.Insert( "Certificados", new{
            Tipo     = 1,
            Estado   = 1,
            IdRoot   = 0, 
            IdUser   = cboUser,
            Pais     = edtC,
            Zona     = edtZ,
            Organo   = edtO,           
            Nombre   = edtCN,
            Correo   = email,
            FechaIni = fechaIni,
            FechaFin = fechaFin
        } );

        //-------------------------------------------------------------------------------------------
        // Rutear pfx
        //-------------------------------------------------------------------------------------------
        //var path1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/bind");
        var path2 = Path.Combine(Directory.GetCurrentDirectory(), "Repositor");
        var final = Path.Combine(path2, string.Format("r{0:D4}.pfx",Id) );

        // Subject = "CN=DigiCert CA-Root, OU=CA CERT, O=CA CERT";
        string Subject = string.Format( "CN={0},O={1},OU={1},L={2},C={3}", edtCN, edtO, edtZ, edtC );
        //-------------------------------------------------------------------------------------------

        // se crea el Certificado e inmediatamente guardarlo en la ruta seteada
        //
        LxTools.X509Generator.X509CertificatesX.GenerateCARoot( Subject, final, "finesi++" );//Clave );

        return Redirect("/panel/caroot");
    }        

    public IActionResult DoAddCert2( int cboRaiz, string edtCN, string edtO, string edtC, string edtZ, string email, string Clave )
    {
        SessData? sess = HttpContext.Session.Get<SessData>("tmpSess");
        if(sess==null) return RedirectToAction( "Index", "Home");
        //-------------------------------------------------------------------

        // Cert tipo 2 : de firma digital necesita de un CertRaiz
        //
        edtCN = LxSess.SecureStr( edtCN );
        edtO  = LxSess.SecureStr( edtO );
        edtC  = LxSess.SecureStr( edtC );
        edtZ  = LxSess.SecureStr( edtZ );
        email = LxSess.SecureStr( email );
        //string? inputValue = Request.Form["edtCN"];


        DateTime fechaIni = DateTime.Now;
        DateTime fechaFin = fechaIni.AddMonths(3); // Agregar tres meses

        
        var Id = dbMini.Insert( "Certificados", new{
            Tipo     = 2,
            Estado   = 1,
            IdRoot   = cboRaiz, 
            IdUser   = sess.Id,
            Pais     = edtC,
            Zona     = edtZ,
            Organo   = edtO,           
            Nombre   = edtCN,
            Correo   = email,
            FechaIni = fechaIni,
            FechaFin = fechaFin
        } );

        //-------------------------------------------------------------------------------------------
        // Rutear pfx
        //-------------------------------------------------------------------------------------------
        var path2 = Path.Combine(Directory.GetCurrentDirectory(), "Repositor");
        var final = Path.Combine(path2, string.Format("c{0:D4}.pfx",Id) );

        string Subject = string.Format( "CN={0},O={1},OU={1},L={2},C={3}", edtCN, edtO, edtZ, edtC );
        string pfxRoot = Path.Combine(path2, string.Format("r{0:D4}.pfx",cboRaiz) );

        LxTools.X509Generator.X509CertificatesX.CreateDigitalCert( Subject, final, Clave, 3, pfxRoot, "finesi++" );

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Descargar( int pid=0 )
    {
        string archPfx;

        var cert = dbMini.GetRow("Certificados", new{ Id = pid });
        if( cert==null)
        {
        }

        if( cert.Tipo == 1 )
            archPfx = string.Format("R{0:D4}.pfx", cert.Id );
        else
            archPfx = string.Format("C{0:D4}.pfx", cert.Id );        

        var path2 = Path.Combine(Directory.GetCurrentDirectory(), "Repositor");
        var final = Path.Combine(path2, archPfx );
        
        try
        {
            if (!System.IO.File.Exists(final))
            {
                return NotFound($"El archivo '{final}' no existe.");
            }

            var archivoBytes = System.IO.File.ReadAllBytes(final);

            if( cert.Tipo == 1 )
                return File(archivoBytes, "application/octet-stream", $"CARoot{cert.Id}.pfx" );//final);
            else
                return File(archivoBytes, "application/octet-stream", $"CertDig{cert.Id}.pfx" );//final);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al descargar el archivo: {ex.Message}");
        }
    }

    [HttpGet]
    public IActionResult BorraR( int pid=0 )
    {
        // solo actualizamos a Estado=0, 
        dbMini.Update( "Certificados", "Estado=0", $"Id={pid}" );
        return Redirect("/panel/caroot");
    }

    public IActionResult BorraC( int pid=0 )
    {
        // solo actualizamos a Estado=0, 
        dbMini.Update( "Certificados", "Estado=0", $"Id={pid}" );
        return Redirect("/panel/index");
    }
}

