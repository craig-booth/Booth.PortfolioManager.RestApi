using System;
using System.Collections.Generic;
using System.Text;

using Booth.Common;

namespace Booth.PortfolioManager.RestApi.Transactions
{

    public enum TransactionType { Aquisition, CashTransaction, CostBaseAdjustment, Disposal, IncomeReceived, OpeningBalance, ReturnOfCapital, UnitCountAdjustment }
    public abstract class Transaction
    {
        public Guid Id { get; set; }
        public abstract TransactionType Type { get; }
        public Guid Stock { get; set; }
        public Date TransactionDate { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
    }

 /*   public static class RestApiNameMapping
    {
        public static string ToRestName(this BankAccountTransactionType transactionType)
        {
            if (transactionType == BankAccountTransactionType.Deposit)
                return "deposit";
            else if (transactionType == BankAccountTransactionType.Withdrawl)
                return "withdrawl";
            else if (transactionType == BankAccountTransactionType.Transfer)
                return "transfer";
            else if (transactionType == BankAccountTransactionType.Fee)
                return "fee";
            else if (transactionType == BankAccountTransactionType.Interest)
                return "interest";
            else
                return "";
        }

        public static BankAccountTransactionType ToBankAccountTransactionType(string transactionType)
        {
            if (transactionType == "deposit")
                return BankAccountTransactionType.Deposit;
            else if (transactionType == "withdrawl")
                return BankAccountTransactionType.Withdrawl;
            else if (transactionType == "transfer")
                return BankAccountTransactionType.Transfer;
            else if (transactionType == "fee")
                return BankAccountTransactionType.Fee;
            else if (transactionType == "interest")
                return BankAccountTransactionType.Interest;
            else
                throw new IndexOutOfRangeException();
        }

       
        public static TransactionType ToTransactionType(string transactionType)
        {
            if (transactionType == "aquisition")
                return TransactionType.Aquisition;
            else if (transactionType == "disposal")
                return TransactionType.Disposal;
            else if (transactionType == "costbaseadjustment")
                return TransactionType.CostBaseAdjustment;
            else if (transactionType == "openingbalance")
                return TransactionType.OpeningBalance;
            else if (transactionType == "returnofcapital")
                return TransactionType.ReturnOfCapital;
            else if (transactionType == "incomereceived")
                return TransactionType.Income;
            else if (transactionType == "unitcountadjustment")
                return TransactionType.UnitCountAdjustment;
            else if (transactionType == "cashtransaction")
                return TransactionType.CashTransaction;
            else
                throw new IndexOutOfRangeException();
        }

        public static string ToRestName(this CGTCalculationMethod cgtMethod)
        {
            if (cgtMethod == CGTCalculationMethod.FirstInFirstOut)
                return "fifo";
            else if (cgtMethod == CGTCalculationMethod.LastInFirstOut)
                return "lifo";
            else if (cgtMethod == CGTCalculationMethod.MaximizeGain)
                return "maximise";
            else if (cgtMethod == CGTCalculationMethod.MinimizeGain)
                return "minimize";

            return "";
        }

        public static CGTCalculationMethod ToCGTCalculationMethod(string cgtMethod)
        {
            if (cgtMethod == "fifo")
                return CGTCalculationMethod.FirstInFirstOut;
            else if (cgtMethod == "lifo")
                return CGTCalculationMethod.LastInFirstOut;
            else if (cgtMethod == "maximise")
                return CGTCalculationMethod.MaximizeGain;
            else if (cgtMethod == "minimize")
                return CGTCalculationMethod.MinimizeGain;
            else
                throw new IndexOutOfRangeException();
        } 
    } */
}
