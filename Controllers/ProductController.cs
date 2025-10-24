
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

// Note: This controller is only responsible for serving the main shop HTML/JS page.
// All cart operations (Add, Update, Checkout) are handled by the separate CartController.

public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;

    // You can inject services here if you needed product data, but for now, we just need the logger.
    public ProductController(ILogger<ProductController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Serves the main e-commerce shop page.
    /// This action is now the default route for the application thanks to the update in Program.cs.
    /// </summary>
    public IActionResult Index()
    {
        _logger.LogInformation("ProductController Index action executed. Attempting to render /Views/Product/Index.cshtml.");

        // 'return View()' tells the MVC framework to look for a view file 
        // that matches the controller name (Product) and the action name (Index).
        // Target Path: /Views/Product/Index.cshtml
        return View();
    }

    /// <summary>
    /// Example of a generic Error page handling.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

// --- DTO for Error Handling (Needed if your MVC project uses default templates) ---

/// <summary>
/// Simple view model to handle error page display.
/// </summary>
public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}