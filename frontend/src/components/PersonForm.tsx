import { useState, type FormEvent } from "react";
import type { CreatePerson } from "../types/person";

interface PersonFormProps {
  isSubmitting: boolean;
  onSubmit: (person: CreatePerson) => Promise<boolean>;
}

export function PersonForm({
  isSubmitting,
  onSubmit,
}: PersonFormProps) {
  const [name, setName] = useState("");
  const [age, setAge] = useState("");

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const normalizedName = name.trim();
    const parsedAge = Number(age);

    if (!normalizedName || !Number.isInteger(parsedAge)) {
      return;
    }

    const wasCreated = await onSubmit({
      name: normalizedName,
      age: parsedAge,
    });

    if (wasCreated) {
      setName("");
      setAge("");
    }
  }

  return (
    <section className="card">
      <div className="section-heading">
        <div>
          <span className="eyebrow">Novo cadastro</span>
          <h2>Adicionar pessoa</h2>
        </div>
      </div>

      <form className="person-form" onSubmit={handleSubmit}>
        <label className="form-field">
          <span>Nome</span>

          <input
            type="text"
            value={name}
            onChange={(event) => setName(event.target.value)}
            maxLength={100}
            placeholder="Ex.: Ana Silva"
            required
            disabled={isSubmitting}
          />
        </label>

        <label className="form-field">
          <span>Idade</span>

          <input
            type="number"
            value={age}
            onChange={(event) => setAge(event.target.value)}
            min={0}
            max={120}
            placeholder="Ex.: 25"
            required
            disabled={isSubmitting}
          />
        </label>

        <button
          className="primary-button"
          type="submit"
          disabled={isSubmitting}
        >
          {isSubmitting ? "Cadastrando..." : "Cadastrar pessoa"}
        </button>
      </form>
    </section>
  );
}