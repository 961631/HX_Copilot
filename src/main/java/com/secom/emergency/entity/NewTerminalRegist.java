package com.secom.emergency.entity;

import jakarta.persistence.*;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Pattern;
import jakarta.validation.constraints.Size;
import java.time.LocalDateTime;

@Entity
@Table(name = "NEW_TERMINAL_REGIST")
public class NewTerminalRegist {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    @NotBlank(message = "端末製造番号は必須です")
    @Size(min = 15, max = 15, message = "端末製造番号は15桁で入力してください")
    @Pattern(regexp = "\\d{15}", message = "端末製造番号は15桁の数字で入力してください")
    @Column(name = "TERMINAL_SERIAL_NUMBER", length = 15, unique = true, nullable = false)
    private String terminalSerialNumber;
    
    @NotBlank(message = "機器PINコードは必須です")
    @Size(min = 5, max = 5, message = "機器PINコードは5桁で入力してください")
    @Pattern(regexp = "\\d{5}", message = "機器PINコードは5桁の数字で入力してください")
    @Column(name = "DEVICE_PIN_CODE", length = 5, nullable = false)
    private String devicePinCode;
    
    @NotBlank(message = "商品コードは必須です")
    @Size(min = 8, max = 8, message = "商品コードは8桁で入力してください")
    @Pattern(regexp = "[A-Za-z0-9]{8}", message = "商品コードは8桁の英数字で入力してください")
    @Column(name = "PRODUCT_CODE", length = 8, nullable = false)
    private String productCode;
    
    @Column(name = "CREATED_AT", nullable = false)
    private LocalDateTime createdAt;
    
    @Column(name = "UPDATED_AT")
    private LocalDateTime updatedAt;
    
    public NewTerminalRegist() {
        this.createdAt = LocalDateTime.now();
    }
    
    public NewTerminalRegist(String terminalSerialNumber, String devicePinCode, String productCode) {
        this();
        this.terminalSerialNumber = terminalSerialNumber;
        this.devicePinCode = devicePinCode;
        this.productCode = productCode;
    }
    
    @PreUpdate
    public void preUpdate() {
        this.updatedAt = LocalDateTime.now();
    }
    
    // Getters and Setters
    public Long getId() {
        return id;
    }
    
    public void setId(Long id) {
        this.id = id;
    }
    
    public String getTerminalSerialNumber() {
        return terminalSerialNumber;
    }
    
    public void setTerminalSerialNumber(String terminalSerialNumber) {
        this.terminalSerialNumber = terminalSerialNumber;
    }
    
    public String getDevicePinCode() {
        return devicePinCode;
    }
    
    public void setDevicePinCode(String devicePinCode) {
        this.devicePinCode = devicePinCode;
    }
    
    public String getProductCode() {
        return productCode;
    }
    
    public void setProductCode(String productCode) {
        this.productCode = productCode;
    }
    
    public LocalDateTime getCreatedAt() {
        return createdAt;
    }
    
    public void setCreatedAt(LocalDateTime createdAt) {
        this.createdAt = createdAt;
    }
    
    public LocalDateTime getUpdatedAt() {
        return updatedAt;
    }
    
    public void setUpdatedAt(LocalDateTime updatedAt) {
        this.updatedAt = updatedAt;
    }
}