using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodkidnoiDurakGame.Core
{
    enum GameError:int
    {
        ActionRefused = 100, // Is not displayed to user
        Warning = 200,
        Error = 300,
        FatalError = 400
    }
}
