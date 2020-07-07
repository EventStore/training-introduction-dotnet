using System;
using System.Collections.Generic;

namespace Application.Domain.ReadModel
{
    public interface IAvailableSlotsRepository
    {
        public void Add(AvailableSlot slot);
        public void MarkAsUnavailable(string slotId);
        public void MarkAsAvailable(string slotId);
        public List<AvailableSlot> getSlotsAvailableOn(DateTime date);
    }
}