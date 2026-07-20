import { useEffect, useState } from "react";
import { PersonForm } from "../components/PersonForm";
import { PersonList } from "../components/PersonList";
import { personService } from "../services/personService";
import type { CreatePerson, Person } from "../types/person";
import { getErrorMessage } from "../utils/getErrorMessage";

export function PersonsPage() {
  const [persons, setPersons] = useState<Person[]>([]);
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
          isSubmitting={isSubmitting}
          onSubmit={handleCreate}
        />

        {isLoading ? (
          <section className="card loading-card">
            <p>Carregando pessoas...</p>
          </section>
        ) : (
          <PersonList
            persons={persons}
            deletingPersonId={deletingPersonId}
            onDelete={handleDelete}
          />
        )}
      </div>
    </div>
  );
}