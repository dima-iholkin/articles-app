namespace ArticlesApp.WebAPI._AppConfiguration.Sections.AppRunMode;



public static class RunModeBuilder
{
    public static RunModeEnum Build(IConfiguration hostConfig)
    {
        string? runMode = hostConfig.GetValue<string?>("RunMode");

        return runMode switch
        {
            "Normal" => RunModeEnum.Normal,
            "DbFill" => RunModeEnum.DbInitializer,
            "DbReaper" => RunModeEnum.DbReaper,
            _ => FallThroughCase(runMode)
        };
    }



    private static RunModeEnum FallThroughCase(string? runMode)
    {
        if (runMode == null)
        {
            Console.Write("RunMode configuration value was not found.");
        }
        else
        {
            Console.Write($"RunMode configuration parameter had an unexpected value: {runMode}.");
        }

        Console.WriteLine(" The application will start in the Normal mode." + Environment.NewLine);
        return RunModeEnum.Normal;
    }
}