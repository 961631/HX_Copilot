package com.secom.emergency.entity;

import jakarta.persistence.*;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Pattern;
import jakarta.validation.constraints.Size;
import java.time.LocalDateTime;

@Entity
@Table(name = "TERMINAL_SIM_HIMODUKE", 
       uniqueConstraints = @UniqueConstraint(columnNames = {"IMEI", "ICCID"}))
public class TerminalSimHimoduke {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    @NotBlank(message = "端末製造番号(IMEI)は必須です")
    @Size(min = 15, max = 15, message = "端末製造番号(IMEI)は15桁で入力してください")
    @Pattern(regexp = "\\d{15}", message = "端末製造番号(IMEI)は15桁の数字で入力してください")
    @Column(name = "IMEI", length = 15, nullable = false)
    private String imei;
    
    @NotBlank(message = "ICカード番号(ICCID)は必須です")
    @Size(min = 19, max = 19, message = "ICカード番号(ICCID)は19桁で入力してください")
    @Pattern(regexp = "\\d{19}", message = "ICカード番号(ICCID)は19桁の数字で入力してください")
    @Column(name = "ICCID", length = 19, nullable = false)
    private String iccid;
    
    @Column(name = "CREATED_AT", nullable = false)
    private LocalDateTime createdAt;
    
    @Column(name = "UPDATED_AT")
    private LocalDateTime updatedAt;
    
    public TerminalSimHimoduke() {
        this.createdAt = LocalDateTime.now();
    }
    
    public TerminalSimHimoduke(String imei, String iccid) {
        this();
        this.imei = imei;
        this.iccid = iccid;
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
    
    public String getImei() {
        return imei;
    }
    
    public void setImei(String imei) {
        this.imei = imei;
    }
    
    public String getIccid() {
        return iccid;
    }
    
    public void setIccid(String iccid) {
        this.iccid = iccid;
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