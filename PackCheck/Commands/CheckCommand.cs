using PackCheck.Commands.Settings;
using Spectre.Console.Cli;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using PackCheck.Data;
using PackCheck.Exceptions;
using PackCheck.Services;
using Spectre.Console;

namespace PackCheck.Commands
{
    public class CheckCommand : AsyncCommand<CheckSettings>
    {
        private readonly CsProjFileService _csProjFileService;
        private readonly NuGetPackagesService _nuGetPackagesService;
        private readonly List<Package> _packages;
        private string _pathToCsProjFile = string.Empty;

        public CheckCommand()
        {
            _csProjFileService = new CsProjFileService();
            _nuGetPackagesService = new NuGetPackagesService();
            _packages = new List<Package>();
        }

        public override async Task<int> ExecuteAsync(CommandContext context, CheckSettings settings)
        {
            try
            {
                _pathToCsProjFile = _csProjFileService.GetPathToCsProjFile(settings.PathToCsProjFile);
            }
            catch (CsProjFileNotFoundException ex)
            {
                AnsiConsole.Markup($"[dim]WARN:[/] [red]{ex.Message}[/]");
                return await Task.FromResult(-1);
            }

            await _nuGetPackagesService.GetPackagesDataFromCsProjFile(_pathToCsProjFile, _packages);
            await _nuGetPackagesService.GetPackagesDataFromNugetRepository(_pathToCsProjFile, _packages);

            PrintResult();

            return await Task.FromResult(0);
        }

        private void PrintResult()
        {
            var table = new Table();

            table.AddColumn("Package Name");
            table.AddColumn("Current Version");
            table.AddColumn("Latest Stable Version");
            table.AddColumn("Latest Version");

            foreach (Package p in _packages)
            {
                table.AddRow(
                    p.PackageName,
                    p.CurrentVersion.ToString(),
                    p.LatestStableVersion?.ToString() ?? "k.A.",
                    p.LatestVersion?.ToString() ?? "k.A."
                );
            }

            table.Columns[1].RightAligned();
            table.Columns[2].RightAligned();
            table.Columns[3].RightAligned();

            AnsiConsole.Render(table);

            Console.WriteLine();
            AnsiConsole.MarkupLine(
                "[dim]INFO:[/] Run [blue]pack-check -u[/] to update the .csproj file with the latest stable versions.");
            AnsiConsole.MarkupLine(
                "[dim]INFO:[/] Run [blue]pack-check -u --pre[/] to update the .csproj file with the latest versions.");
            AnsiConsole.MarkupLine(
                "[dim]INFO:[/] Run [blue]pack-check -u -p <Package Name>[/] to update only the specified package to the latest stable version.");
            AnsiConsole.MarkupLine(
                "[dim]INFO:[/] Run [blue]pack-check -u --pre -p <Package Name>[/] to update only the specified package to the latest version.");
            Console.WriteLine();
        }
    }
}