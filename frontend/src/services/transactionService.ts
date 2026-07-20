import { api } from "./api";
import type {
  CreateTransaction,
  Transaction,
} from "../types/transaction";

const endpoint = "/transactions";

export const transactionService = {
  async getAll(): Promise<Transaction[]> {
    const response = await api.get<Transaction[]>(endpoint);

    return response.data;
  },

  async create(
    transaction: CreateTransaction,
  ): Promise<Transaction> {
    const response = await api.post<Transaction>(
      endpoint,
      transaction,
    );

    return response.data;
  },
};