import { useState, type FormEvent } from "react";
import type {
  CreatePerson,
  Person,
  UpdatePerson,
} from "../types/person";

interface PersonFormProps {
  editingPerson: Person | null;
  isSubmitting: boolean;
  onCreate: (person: CreatePerson) => Promise<boolean>;
  onUpdate: (
    id: number,
    person: UpdatePerson,
  ) => Promise<boolean>;
  onCancelEdit: () => void;
}

export function PersonForm({
  editingPerson,
  isSubmitting,
  onCreate,
  onUpdate,
  onCancelEdit,
}: PersonFormProps) {
  const [name, setName] = useState(
    editingPerson?.name ?? "",
  );

  const [age, setAge] = useState(
    editingPerson ? String(editingPerson.age) : "",
  );

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const normalizedName = name.trim();
    const parsedAge = Number(age);

    if (!normalizedName || !Number.isInteger(parsedAge)) {
      return;
    }

    const personData = {
      name: normalizedName,
      age: parsedAge,
    };

    // A edição reaproveita o cadastro atual e preserva o mesmo ID.
    const wasSaved = editingPerson
      ? await onUpdate(editingPerson.id, personData)
      : await onCreate(personData);

    // Os campos são limpos apenas quando o backend confirma a operação.
    if (wasSaved) {
      setName("");
      setAge("");
    }
  }

  function handleCancel() {
    setName("");
    setAge("");
    onCancelEdit();
  }

  return (
    <section className="card">
      <div className="section-heading">
        <div>
          <span className="eyebrow">
            {editingPerson ? "Editar cadastro" : "Novo cadastro"}
          </span>

          <h2>
            {editingPerson
              ? "Atualizar pessoa"
              : "Adicionar pessoa"}
          </h2>
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

        <div className="form-actions">
          <button
            className="primary-button"
            type="submit"
            disabled={isSubmitting}
          >
            {isSubmitting
              ? "Salvando..."
              : editingPerson
                ? "Salvar alterações"
                : "Cadastrar pessoa"}
          </button>

          {editingPerson && (
            <button
              className="secondary-button"
              type="button"
              disabled={isSubmitting}
              onClick={handleCancel}
            >
              Cancelar
            </button>
          )}
        </div>
      </form>
    </section>
  );
}