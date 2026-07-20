import axios from "axios";

interface ApiErrorResponse {
  title?: string;
  detail?: string;
  errors?: Record<string, string[]>;
}

export function getErrorMessage(
  error: unknown,
  fallbackMessage: string,
): string {
  if (!axios.isAxiosError<ApiErrorResponse>(error)) {
    return fallbackMessage;
  }

  const response = error.response?.data;

  if (response?.detail) {
    return response.detail;
  }

  if (response?.errors) {
    const firstValidationMessage = Object
      .values(response.errors)
      .flat()
      .at(0);

    if (firstValidationMessage) {
      return firstValidationMessage;
    }
  }

  return fallbackMessage;
}