import { useEffect, useState } from "react";
import { personService } from "./services/personService";
import type { Person } from "./types/person";

function App() {
  const [persons, setPersons] = useState<Person[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadPersons() {
      try {
        const data = await personService.getAll();

        setPersons(data);
      } catch {
        setError("Não foi possível conectar ao backend.");
      } finally {
        setIsLoading(false);
      }
    }

    void loadPersons();
  }, []);

  return (
    <main className="app">
      <h1>Controle de Gastos</h1>

      {isLoading && <p>Carregando pessoas...</p>}

      {error && <p>{error}</p>}

      {!isLoading && !error && (
        <p>
          Backend conectado. Pessoas cadastradas:{" "}
          <strong>{persons.length}</strong>
        </p>
      )}
    </main>
  );
}

export default App;