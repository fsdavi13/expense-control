import { useEffect, useState } from "react";
import { PersonForm } from "../components/PersonForm";
import { PersonList } from "../components/PersonList";
import { personService } from "../services/personService";
import type {
  CreatePerson,
  Person,
  UpdatePerson,
} from "../types/person";
import { getErrorMessage } from "../utils/getErrorMessage";

export function PersonsPage() {
  const [persons, setPersons] = useState<Person[]>([]);
  const [editingPerson, setEditingPerson] =
    useState<Person | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [deletingPersonId, setDeletingPersonId] = useState<
    number | null
  >(null);
  const [error, setError] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  useEffect(() => {
    let isCancelled = false;

    personService
      .getAll()
      .then((data) => {
        if (!isCancelled) {
          setPersons(data);
        }
      })
      .catch((requestError: unknown) => {
        if (!isCancelled) {
          setError(
            getErrorMessage(
              requestError,
              "Não foi possível carregar as pessoas.",
            ),
          );
        }
      })
      .finally(() => {
        if (!isCancelled) {
          setIsLoading(false);
        }
      });

    return () => {
      isCancelled = true;
    };
  }, []);

  async function handleCreate(
    person: CreatePerson,
  ): Promise<boolean> {
    try {
      setIsSubmitting(true);
      setError("");
      setSuccessMessage("");

      const createdPerson = await personService.create(person);

      setPersons((currentPersons) => [
        ...currentPersons,
        createdPerson,
      ]);

      setSuccessMessage("Pessoa cadastrada com sucesso.");

      return true;
    } catch (requestError) {
      setError(
        getErrorMessage(
          requestError,
          "Não foi possível cadastrar a pessoa.",
        ),
      );

      return false;
    } finally {
      setIsSubmitting(false);
    }
  }

  async function handleUpdate(
    id: number,
    person: UpdatePerson,
  ): Promise<boolean> {
    try {
      setIsSubmitting(true);
      setError("");
      setSuccessMessage("");

      const updatedPerson = await personService.update(
        id,
        person,
      );

      // A lista é atualizada mantendo o mesmo registro e identificador.
      setPersons((currentPersons) =>
        currentPersons.map((currentPerson) =>
          currentPerson.id === id
            ? updatedPerson
            : currentPerson,
        ),
      );

      setEditingPerson(null);
      setSuccessMessage("Pessoa atualizada com sucesso.");

      return true;
    } catch (requestError) {
      setError(
        getErrorMessage(
          requestError,
          "Não foi possível atualizar a pessoa.",
        ),
      );

      return false;
    } finally {
      setIsSubmitting(false);
    }
  }

  function handleEdit(person: Person) {
    setError("");
    setSuccessMessage("");
    setEditingPerson(person);

    window.scrollTo({
      top: 0,
      behavior: "smooth",
    });
  }

  function handleCancelEdit() {
    setEditingPerson(null);
    setError("");
  }

  async function handleDelete(person: Person) {
    const confirmed = window.confirm(
      `Excluir ${person.name}? As transações dessa pessoa também serão removidas.`,
    );

    if (!confirmed) {
      return;
    }

    try {
      setDeletingPersonId(person.id);
      setError("");
      setSuccessMessage("");

      await personService.delete(person.id);

      setPersons((currentPersons) =>
        currentPersons.filter(
          (currentPerson) => currentPerson.id !== person.id,
        ),
      );

      if (editingPerson?.id === person.id) {
        setEditingPerson(null);
      }

      setSuccessMessage("Pessoa excluída com sucesso.");
    } catch (requestError) {
      setError(
        getErrorMessage(
          requestError,
          "Não foi possível excluir a pessoa.",
        ),
      );
    } finally {
      setDeletingPersonId(null);
    }
  }

  return (
    <div className="page">
      <header className="page-header">
        <div>
          <span className="eyebrow">Controle residencial</span>
          <h1>Pessoas</h1>
          <p>
            Gerencie os moradores que poderão registrar receitas e
            despesas.
          </p>
        </div>

        <div className="header-summary">
          <span>Total cadastrado</span>
          <strong>{persons.length}</strong>
        </div>
      </header>

      {error && (
        <div className="feedback feedback-error" role="alert">
          {error}
        </div>
      )}

      {successMessage && (
        <div className="feedback feedback-success" role="status">
          {successMessage}
        </div>
      )}

      <div className="content-grid">
        <PersonForm
          key={editingPerson?.id ?? "new-person"}
          editingPerson={editingPerson}
          isSubmitting={isSubmitting}
          onCreate={handleCreate}
          onUpdate={handleUpdate}
          onCancelEdit={handleCancelEdit}
        />

        {isLoading ? (
          <section className="card loading-card">
            <p>Carregando pessoas...</p>
          </section>
        ) : (
          <PersonList
            persons={persons}
            deletingPersonId={deletingPersonId}
            editingPersonId={editingPerson?.id ?? null}
            onEdit={handleEdit}
            onDelete={handleDelete}
          />
        )}
      </div>
    </div>
  );
}