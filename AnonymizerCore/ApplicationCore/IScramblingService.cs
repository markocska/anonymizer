using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler
{
    public interface IScramblingService
    {
        void ScrambleFromConfigStr(string configStr);

        void ScrambleFromConfigPath(string configPath);
    }
}
