using System;
using System.Collections.Generic;

namespace Model
{
    public class Track
    {
        public string Name;
        public LinkedList<Section> Sections;

        public Track(string name, SectionTypes[] sections)
        {
            Name = name;
            Sections = SetSections(sections);
        }

        private LinkedList<Section> SetSections(SectionTypes[] sections)
        {
            LinkedList<Section> sectionList = new LinkedList<Section>();

            foreach (var section in sections)
            {
                sectionList.AddLast(new Section(section));
            }
            
            return sectionList;
        }
    }
}
