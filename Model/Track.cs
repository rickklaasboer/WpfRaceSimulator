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

        public Section GetNextSection(Section currentSection)
        {
            LinkedListNode<Section> node = Sections.Find(currentSection);

            if (node != null && node.Next != null)
            {
                return node.Next.Value;   
            }

            return null;
        }

        public Section GetPreviousSection(Section currentSection)
        {
            LinkedListNode<Section> node = Sections.Find(currentSection);

            if (node != null && node.Previous != null)
            {
                return node.Previous.Value;   
            }

            return null;
        }
    }
}
