public static class InventoryExtensions
{
    public static void TryMoveToInv(this Inventory targetInv, InventorySlot sourceSlot)
    {
        var left = targetInv.AddItem(sourceSlot.item, sourceSlot.numberOfItem);
        if (left == 0)
        {
            sourceSlot.item = null;
            sourceSlot.numberOfItem = 0;
        }
        else
        {
            sourceSlot.numberOfItem = left;
        }
    }
}