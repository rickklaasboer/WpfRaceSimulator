using System;
using System.Collections.Generic;

namespace Model
{
    public class Track
    {
        public string Name;
        public LinkedList<Section> Sections;

        public Track(string name, LinkedList<Section> sections)
        {
            Name = name;
            Sections = sections;
        }
    }
}
