import { api } from "./api";
import type { TotalsResponse } from "../types/totals";

const endpoint = "/totals";

export const totalsService = {
  async get(): Promise<TotalsResponse> {
    const response = await api.get<TotalsResponse>(endpoint);

    return response.data;
  },
};