export interface PersonTotals {
  personId: number;
  personName: string;
  totalIncome: number;
  totalExpense: number;
  balance: number;
}

export interface GeneralTotals {
  totalIncome: number;
  totalExpense: number;
  balance: number;
}

export interface TotalsResponse {
  persons: PersonTotals[];
  general: GeneralTotals;
}