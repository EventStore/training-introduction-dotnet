using System;
using System.Collections.Generic;

namespace Application.Domain.ReadModel
{
    public interface IAvailableSlotsRepository
    {
        void Add(AvailableSlot slot);
        void MarkAsUnavailable(string slotId);
        void MarkAsAvailable(string slotId);
        List<AvailableSlot> getSlotsAvailableOn(DateTime date);
        void Clear();
    }
}
