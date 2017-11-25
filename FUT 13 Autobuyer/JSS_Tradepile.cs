using System.Collections.Generic;

namespace JSS_Tradepile
{
    public class AttributeList
    {
        public int value { get; set; }
        public int index { get; set; }
    }

    public class StatsList
    {
        public int value { get; set; }
        public int index { get; set; }
    }

    public class LifetimeStat
    {
        public int value { get; set; }
        public int index { get; set; }
    }

    public class ItemData
    {
        public long id { get; set; }
        public string timestamp { get; set; }
        public string itemType { get; set; }
        public int training { get; set; }
        public int rating { get; set; }
        public int teamid { get; set; }
        public string formation { get; set; }
        public string itemState { get; set; }
        public string resourceId { get; set; }
        public int lastSalePrice { get; set; }
        public int owners { get; set; }
        public string discardValue { get; set; }
        public string injuryType { get; set; }
        public int injuryGames { get; set; }
        public int suspension { get; set; }
        public int morale { get; set; }
        public int fitness { get; set; }
        public int cardsubtypeid { get; set; }
        public string preferredPosition { get; set; }
        public List<AttributeList> attributeList { get; set; }
        public List<StatsList> statsList { get; set; }
        public List<LifetimeStat> lifetimeStats { get; set; }
        public int assetId { get; set; }
        public int contract { get; set; }
        public int rareflag { get; set; }
    }

    public class AuctionInfo
    {
        public string tradeId { get; set; }
        public string tradeState { get; set; }
        public int offers { get; set; }
        public ItemData itemData { get; set; }
        public object bidState { get; set; }
        public int buyNowPrice { get; set; }
        public bool watched { get; set; }
        public int startingBid { get; set; }
        public int currentBid { get; set; }
        public int expires { get; set; }
        public object sellerName { get; set; }
        public int sellerEstablished { get; set; }
        public int sellerId { get; set; }
    }

    public class BidTokens
    {
    }

    public class RootObject
    {
        public List<AuctionInfo> auctionInfo { get; set; }
        public BidTokens bidTokens { get; set; }
        public string currencies { get; set; }
        public int credits { get; set; }
        public string duplicateItemIdList { get; set; }
        public string errorState { get; set; }
    }
}
