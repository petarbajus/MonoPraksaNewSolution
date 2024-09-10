﻿namespace NewSolution.Model
{
    public class Footballer
    {

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateOnly? DOB { get; set; }
        public Guid? ClubId { get; set; }

        public Club? Club { get; set; }
        public Footballer()
        {

        }
    }
}
