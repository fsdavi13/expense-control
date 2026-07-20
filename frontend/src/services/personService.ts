import { api } from "./api";
import type {
  CreatePerson,
  Person,
  UpdatePerson,
} from "../types/person";

const endpoint = "/persons";

export const personService = {
  async getAll(): Promise<Person[]> {
    const response = await api.get<Person[]>(endpoint);

    return response.data;
  },

  async create(person: CreatePerson): Promise<Person> {
    const response = await api.post<Person>(endpoint, person);

    return response.data;
  },

  async update(
    id: number,
    person: UpdatePerson,
  ): Promise<Person> {
    const response = await api.put<Person>(
      `${endpoint}/${id}`,
      person,
    );

    return response.data;
  },

  async delete(id: number): Promise<void> {
    await api.delete(`${endpoint}/${id}`);
  },
};