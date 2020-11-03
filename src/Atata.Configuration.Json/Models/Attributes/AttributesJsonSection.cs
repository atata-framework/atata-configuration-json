using System.Collections.Generic;

namespace Atata.Configuration.Json
{
    public class AttributesJsonSection
    {
        public List<AttributeJsonSection> Global { get; set; }

        public List<AssemblyAttributesJsonSection> Assembly { get; set; }

        public List<ComponentAttributesJsonSection> Component { get; set; }
    }
}
