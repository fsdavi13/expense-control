export const TransactionType = {
  Expense: 1,
  Income: 2,
} as const;

export type TransactionType =
  (typeof TransactionType)[keyof typeof TransactionType];

export interface Transaction {
  id: number;
  description: string;
  amount: number;
  type: TransactionType;
  personId: number;
}

export interface CreateTransaction {
  description: string;
  amount: number;
  type: TransactionType;
  personId: number;
}