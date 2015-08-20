
using System.Collections.Generic;
namespace ModelGeneratorTool.Interfaces
{    
    public interface IModelGenerator
    {
        /// <summary>
        /// Generates model based on table from available database and datasource
        /// </summary>
        /// <param name="userInputs">dynamic type anonymous</param>
        /// <param name="properties">Property Statistics from database</param>
        /// <returns>Result as string</returns>
        System.Threading.Tasks.Task<string> GenerateModelAsync(dynamic userInputs, List<ModelGeneratorTool.Utilities.Property> properties);
        /// <summary>
        /// Copy properties list into Clipboard to avoid boiler plate code for backing fields and properties
        /// </summary>
        /// <param name="properties">Property list</param>
        /// <param name="backingFieldRequired">backing field if required</param>
        /// <returns>returns string of properties</returns>
        string CopyPropertyStrings(List<ModelGeneratorTool.Utilities.Property> properties, bool backingFieldRequired);
    }
}
