using System;
using System.Collections.ObjectModel;
using System.Data;
using NUnit.Framework;

namespace BingoLib.Tests
{
   public class BingoExtensionTest
   {
      [SetUp]
      public void Setup()
      {
      }

      [Test]
      public void IsNumeric_returns_False_If_A_Text_is_passed() {
         const string testVal = "Pippo";

         Assert.IsFalse(testVal.IsNumeric());
      }

      [TestCase("0b00010001")]
      [TestCase("0b_0001_0001")]
      [TestCase("123456789")]
      [TestCase("0777")]
      [TestCase("0_777")]
      [TestCase("0xffa0bc00")]
      [TestCase("0x_ff_a0_bc_00")]
      [TestCase("0xFFA0BC00")]
      [TestCase("0x_FFA0_BC00")]
      public void IsNumeric_returns_false_With_numeric_values(string testValue) {
         Assert.IsTrue(testValue.IsNumeric());
      }

      [TestCase("", true)]
      [TestCase(" ", true)]
      [TestCase("\t", true)]
      [TestCase("\t \r\n", true)]
      [TestCase("pippo", false)]
      public void Testing_IsBlank(string testVal, bool expected) {
         bool result = testVal.IsBlank();
         Assert.AreEqual(result, expected);
      }

      [Test]
      public void BeginOfMonth_Works_with_Date_Value() {
         var passedDate = new DateTime(2019, 2, 25);
         var expectedDate = new DateTime(2019, 2, 1);

         Assert.AreEqual(passedDate.MonthBegin(), expectedDate);
      }

      [Test]
      public void EndOfMonth_Works_with_Date_Value()
      {
         var passedDate = new DateTime(2019, 2, 25);
         var expectedDate = new DateTime(2019, 2, 28);

         Assert.AreEqual(passedDate.MonthEnd(), expectedDate);
      }

      [Test]
      public void EndOfMonth_Works_with_Date_Value_In_Leap_Year()
      {
         var passedDate = new DateTime(2020, 2, 25);
         var expectedDate = new DateTime(2020, 2, 29);

         Assert.AreEqual(passedDate.MonthEnd(), expectedDate);
      }

      [Test]
      public void Change_Collection_To_DataTable() {
         var testVal = new Collection<int>() { 1, 2, 3, 4, 5, 6 };
         var dt = testVal.ToDataTable();

         Assert.IsTrue(dt is DataTable);
      }

   }
}