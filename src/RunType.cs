namespace FrogLib;

public enum RunType {

    /// <summary>
    /// updates and renders at the same time as fast as possible, will skip render calls in an attempt to meet the update frequency (renderFrequency does nothing with this type) <br/>
    /// delta will be variable 
    /// </summary>
    VariableCoupled,

    /// <summary>
    /// updates and renders seperately at a rate determined by their respective frequencies, will skip render calls in an attempt to meet the update frequency <br/>
    /// delta will be fixed
    /// </summary>
    FixedDecoupled
}