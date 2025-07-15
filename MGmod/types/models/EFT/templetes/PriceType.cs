namespace _MGMod.types.models.EFTofMG.templetes;

public class PriceType
{
    public required List<int> date {  get; set; }
    public required Dictionary<string,double> prices { get; set; }
}
