using StudyStud.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace StudyStudTests.Utilities
{
    class TopicComparer : IComparer<Topic>
    {
        public int Compare([NotNull] Topic x, [NotNull] Topic y)
        {
            return x.Date.CompareTo(y.Date);
        }
    }
}
