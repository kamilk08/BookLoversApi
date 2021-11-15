// Decompiled with JetBrains decompiler
// Type: BookLovers.Bookcases.Domain.ShelfRecordTracker.BusinessRules.ShelfRecordTrackerShouldNotContainDuplicatedRecords
// Assembly: BookLovers.Bookcases, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF95724B-8DC6-4675-9F2B-BBCAC7CCC5AF
// Assembly location: C:\Users\Kamil\source\repos\BookLovers\BookLovers.Auth.Tests\bin\Release\BookLovers.Bookcases.dll

using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.ShelfRecordTracker.BusinessRules
{
  internal class ShelfRecordTrackerShouldNotContainDuplicatedRecords : IBusinessRule
  {
    private const string BrokenBusinessRuleMessage = "Shelf record tracker should not contain duplicated records";
    private readonly BookLovers.Bookcases.Domain.ShelfRecordTracker.ShelfRecordTracker _tracker;
    private readonly ShelfRecord _record;

    public ShelfRecordTrackerShouldNotContainDuplicatedRecords(
      BookLovers.Bookcases.Domain.ShelfRecordTracker.ShelfRecordTracker tracker,
      ShelfRecord record)
    {
      this._tracker = tracker;
      this._record = record;
    }

    public bool IsFulfilled() => (BookLovers.Base.Domain.ValueObject.ValueObject<ShelfRecord>) this._tracker.GetTrackedBook(this._record.BookGuid, this._record.ShelfGuid) == (BookLovers.Base.Domain.ValueObject.ValueObject<ShelfRecord>) null;

    public string BrokenRuleMessage => "Shelf record tracker should not contain duplicated records";
  }
}
