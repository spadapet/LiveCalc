using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.WinForms;
using System.Diagnostics;
using WarpToolkit.WinForms.AppServices.ServiceExtensions;

namespace CalcWinForms;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        WinFormsApplicationBuilder builder = WinFormsApplication.CreateBuilder();

        // We want to use the UserSettings service, for a convenient
        // way to store user settings in a file.
        builder.Services.AddWinFormsUserSettingsService();

        // We want to use the Exception service, so we can handle
        // unhandled exceptions in a consistent way.
        builder.Services.AddWinFormsExceptionService();

        Debug.Assert(Thread.CurrentThread.GetApartmentState() == ApartmentState.STA);

        builder.Services.AddScoped<MainForm>();

        // Configure WinForms-specific options
        builder.UseStartupForm<MainForm>()
            // Setting HighDpiMode to SystemAware:
            .UseHighDpiMode(HighDpiMode.SystemAware)
            // Setting the DarkMode to System.
            .UseColorMode(SystemColorMode.System)
            // Equals to SetCompatibleTextRenderingDefault(false)
            .UseTextRenderingV2()
            // This is default for Windows 10+ and later.
            .UseVisualStyles();

        // Build and run the application
        WinFormsApplication app = builder.Build();

        app.Run();
    }
}
